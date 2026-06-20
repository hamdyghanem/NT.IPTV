export interface UserProfile {
  profileName: string;
  username: string;
  password: string;
  server: string;
  port: string;
  useHttps: boolean;
}

export interface ApiSession {
  username: string;
  password: string;
  server: string;
  port: string;
  useHttps: boolean;
}

export interface PlayerInfoResponse {
  user_info?: {
    username?: string;
    password?: string;
    status?: string;
    message?: string;
    auth?: number;
    exp_date?: string;
    is_trial?: string;
    active_cons?: string;
    created_at?: string;
    max_connections?: string;
    allowed_output_formats?: string[];
  };
  server_info?: {
    url?: string;
    port?: string;
    https_port?: string;
    server_protocol?: string;
    rtmp_port?: string;
    timezone?: string;
    timestamp_now?: number;
    time_now?: string;
  };
}

export interface StreamCategory {
  category_id: string;
  category_name: string;
  parent_id: string;
}

export interface StreamChannel {
  num: string;
  name: string;
  stream_icon: string;
  stream_id: string | number;
  category_id: string;
  stream_type: string;
  custom_sid?: string;
}

export interface StreamVideo {
  num: string;
  name: string;
  stream_icon: string;
  stream_id: string | number;
  category_id: string;
  stream_type: string;
  added: string;
  releaseDate?: string;
  rating?: string;
  rating_5based?: string;
  container_extension: string;
}

export interface StreamSeries {
  num: string;
  name: string;
  series_id: string | number;
  cover: string;
  plot: string;
  category_id: string;
  rating: string;
  rating_5based?: string;
  releaseDate?: string;
  added: string;
  director: string;
  genre: string;
  last_modified: string;
}

export interface MovieInfo {
  movie_image: string;
  backdrop: string;
  tmdb_id: string;
  plot: string;
  genre: string;
  releaseDate: string;
  director: string;
  youtube_trailer: string;
  cast: string;
  backdrop_path: string[];
  duration: string;
  duration_secs: string | number;
  rating?: string;
  rating_5based?: string;
}

export interface MovieData {
  stream_id: string | number;
  name: string;
  container_extension: string;
  custom_sid: string;
  added: string;
  tags?: string;
}

export interface WatchMovie {
  info: MovieInfo;
  movie_data: MovieData;
}

export interface SeriesInfo {
  name: string;
  cover: string;
  backdrop: string;
  tmdb_id: string;
  plot: string;
  genre: string;
  director: string;
  youtube_trailer: string;
  cast: string;
  backdrop_path: string[];
  duration_secs?: string | number;
  rating?: string;
  rating_5based?: string;
}

export interface SeasonData {
  id: number;
  episode_count: number;
  air_date: string;
  name: string;
  season_number: number;
  overview: string;
  vote_average: string;
  cover: string;
  cover_big: string;
}

export interface EpisodeInfo {
  duration_secs: string | number;
  duration: string;
  movie_image: string;
  plot: string;
  releasedate: string;
}

export interface EpisodeData {
  id: string | number;
  season: number | string;
  episode_num: string | number;
  title: string;
  container_extension: string;
  custom_sid: string;
  added: string;
  info: EpisodeInfo;
}

export interface WatchSeries {
  seasons: SeasonData[];
  info: SeriesInfo;
  episodes: Record<string, EpisodeData[]>;
}
