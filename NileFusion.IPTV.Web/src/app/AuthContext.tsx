import React, { createContext, useContext, useState, useEffect } from 'react';
import { ApiSession, PlayerInfoResponse, UserProfile } from '../types';
import { testConnection, clearCache } from '../services/api';

interface Favorites {
  live: string[];
  movies: string[];
  series: string[];
}

interface HiddenCategories {
  live: string[];
  movies: string[];
  series: string[];
}

interface AuthContextType {
  activeSession: ApiSession | null;
  playerInfo: PlayerInfoResponse | null;
  savedProfiles: UserProfile[];
  favorites: Favorites;
  isLoading: boolean;
  login: (session: ApiSession, saveProfile: boolean, profileName: string) => Promise<void>;
  logout: () => void;
  deleteProfile: (profileName: string) => void;
  toggleFavorite: (type: 'live' | 'movies' | 'series', id: string | number) => void;
  isFavorite: (type: 'live' | 'movies' | 'series', id: string | number) => boolean;
  toggleHiddenCategory: (type: 'live' | 'movies' | 'series', categoryId: string | number) => void;
  isHiddenCategory: (type: 'live' | 'movies' | 'series', categoryId: string | number) => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [activeSession, setActiveSession] = useState<ApiSession | null>(null);
  const [playerInfo, setPlayerInfo] = useState<PlayerInfoResponse | null>(null);
  const [savedProfiles, setSavedProfiles] = useState<UserProfile[]>([]);
  const [favorites, setFavorites] = useState<Favorites>({ live: [], movies: [], series: [] });
  const [hiddenCategories, setHiddenCategories] = useState<HiddenCategories>({ live: [], movies: [], series: [] });
  const [isLoading, setIsLoading] = useState(true);

  // Load profiles and session on mount
  useEffect(() => {
    try {
      const storedProfiles = localStorage.getItem('nilefusion_profiles');
      if (storedProfiles) {
        setSavedProfiles(JSON.parse(storedProfiles));
      }

      const storedSession = localStorage.getItem('nilefusion_active_session');
      const storedPlayerInfo = localStorage.getItem('nilefusion_player_info');
      
      if (storedSession && storedPlayerInfo) {
        const session: ApiSession = JSON.parse(storedSession);
        setActiveSession(session);
        setPlayerInfo(JSON.parse(storedPlayerInfo));
        
        // Load favorites for this session
        const storedFavs = localStorage.getItem(`nilefusion_favs_${session.username}_${session.server}`);
        if (storedFavs) {
          setFavorites(JSON.parse(storedFavs));
        }
        // Load hidden categories for this session
        const storedHidden = localStorage.getItem(`nilefusion_hidden_${session.username}_${session.server}`);
        if (storedHidden) {
          setHiddenCategories(JSON.parse(storedHidden));
        }
      }
    } catch (e) {
      console.error("Failed to restore auth state", e);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const login = async (session: ApiSession, saveProfile: boolean, profileName: string) => {
    try {
      const info = await testConnection(session);
      
      if (!info.user_info || info.user_info.auth !== 1) {
        throw new Error(info.user_info?.message || "Authentication failed");
      }

      if (info.user_info.status?.toLowerCase() === 'expired') {
        throw new Error("Your account has expired");
      }

      // Successful login
      setActiveSession(session);
      setPlayerInfo(info);
      localStorage.setItem('nilefusion_active_session', JSON.stringify(session));
      localStorage.setItem('nilefusion_player_info', JSON.stringify(info));

      // Load or initialize favorites
      const favKey = `nilefusion_favs_${session.username}_${session.server}`;
      let currentFavs: Favorites = { live: [], movies: [], series: [] };
      const storedFavs = localStorage.getItem(favKey);
      if (storedFavs) {
        currentFavs = JSON.parse(storedFavs);
      }
      setFavorites(currentFavs);

      // Load or initialize hidden categories
      const hiddenKey = `nilefusion_hidden_${session.username}_${session.server}`;
      let currentHidden: HiddenCategories = { live: [], movies: [], series: [] };
      const storedHidden = localStorage.getItem(hiddenKey);
      if (storedHidden) {
        currentHidden = JSON.parse(storedHidden);
      }
      setHiddenCategories(currentHidden);

      // Save profile if requested
      if (saveProfile && profileName) {
        setSavedProfiles((prev) => {
          const updated = prev.filter((p) => p.profileName !== profileName);
          const newProfile: UserProfile = {
            profileName,
            ...session,
          };
          const next = [...updated, newProfile];
          localStorage.setItem('nilefusion_profiles', JSON.stringify(next));
          return next;
        });
      }
    } finally {
      // No global loading update needed here as isConnecting is tracked on the login page locally
    }
  };

  const logout = () => {
    setActiveSession(null);
    setPlayerInfo(null);
    setFavorites({ live: [], movies: [], series: [] });
    setHiddenCategories({ live: [], movies: [], series: [] });
    localStorage.removeItem('nilefusion_active_session');
    localStorage.removeItem('nilefusion_player_info');
    clearCache();
  };

  const deleteProfile = (profileName: string) => {
    setSavedProfiles((prev) => {
      const next = prev.filter((p) => p.profileName !== profileName);
      localStorage.setItem('nilefusion_profiles', JSON.stringify(next));
      return next;
    });
  };

  const toggleFavorite = (type: 'live' | 'movies' | 'series', id: string | number) => {
    if (!activeSession) return;
    const strId = String(id);
    const favKey = `nilefusion_favs_${activeSession.username}_${activeSession.server}`;

    setFavorites((prev) => {
      const list = prev[type];
      const isFav = list.includes(strId);
      const updatedList = isFav ? list.filter((item) => item !== strId) : [...list, strId];
      
      const next = {
        ...prev,
        [type]: updatedList,
      };
      
      localStorage.setItem(favKey, JSON.stringify(next));
      return next;
    });
  };

  const isFavorite = (type: 'live' | 'movies' | 'series', id: string | number) => {
    return favorites[type].includes(String(id));
  };

  const toggleHiddenCategory = (type: 'live' | 'movies' | 'series', categoryId: string | number) => {
    if (!activeSession) return;
    const strId = String(categoryId);
    const hiddenKey = `nilefusion_hidden_${activeSession.username}_${activeSession.server}`;

    setHiddenCategories((prev) => {
      const list = prev[type];
      const isHidden = list.includes(strId);
      const updatedList = isHidden ? list.filter((item) => item !== strId) : [...list, strId];

      const next = {
        ...prev,
        [type]: updatedList,
      };

      localStorage.setItem(hiddenKey, JSON.stringify(next));
      return next;
    });
  };

  const isHiddenCategory = (type: 'live' | 'movies' | 'series', categoryId: string | number) => {
    return hiddenCategories[type].includes(String(categoryId));
  };

  return (
    <AuthContext.Provider
      value={{
        activeSession,
        playerInfo,
        savedProfiles,
        favorites,
        isLoading,
        login,
        logout,
        deleteProfile,
        toggleFavorite,
        isFavorite,
        toggleHiddenCategory,
        isHiddenCategory,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
