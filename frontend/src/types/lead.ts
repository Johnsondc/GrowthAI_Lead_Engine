export type LeadStatus = 'New' | 'Contacted' | 'Communicating' | 'HighIntent' | 'Won' | 'Invalid';
export type SourcePlatform = 'Douyin' | 'Xiaohongshu' | 'Wechat' | 'VideoAccount' | 'OfficialAccount' | 'Moments' | 'QrCode' | 'H5Form' | 'MiniProgram';

export interface LeadCustomer {
  id: string;
  name: string;
  phone?: string;
  wechat?: string;
  city?: string;
  sourcePlatform: SourcePlatform;
  sourceAccount?: string;
  consultationContent?: string;
  interestedProduct?: string;
  status: LeadStatus;
  ownerUserId?: string;
  createdAt: string;
}

export interface OverviewMetrics {
  todayNewLeads: number;
  validLeads: number;
  highIntentLeads: number;
  wonLeads: number;
}
