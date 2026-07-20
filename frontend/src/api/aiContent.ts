import { apiPost } from './client';

export interface GenerateContentPayload {
  industry: string;
  productName: string;
  city: string;
  targetAudience: string;
  sellingPoints: string;
  platform: string;
}

export interface GeneratedContent {
  platform: string;
  title: string;
  body: string;
  script: string;
  tags: string;
  callToAction: string;
}

export function generateContent(payload: GenerateContentPayload) {
  return apiPost<GeneratedContent>('/api/ai/content/generate', payload);
}
