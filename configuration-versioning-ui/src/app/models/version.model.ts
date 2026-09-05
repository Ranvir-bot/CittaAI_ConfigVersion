export interface Version {
  id: number;
  configurationId: number;
  versionNumber: number;
  configurationJson: string;
  createdAt: string;
  author: string;
  comment?: string;
  previousVersionId?: number;
}