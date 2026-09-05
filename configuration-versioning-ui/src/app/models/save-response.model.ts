import { Version } from './version.model';
export interface SaveResponse {
  success: boolean;
  message: string;
  version?: Version;
}