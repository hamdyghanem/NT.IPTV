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
  };
  server_info?: {
    url?: string;
    port?: string;
    server_protocol?: string;
  };
}
