import { apiGet, apiPost } from './client';
import type { LeadCustomer } from '../types/lead';

export interface LandingConfig {
  code: string;
  title: string;
  fields: string[];
}

export function getLandingConfig(code: string) {
  return apiGet<LandingConfig>(`/api/landing/${code}`);
}

export function submitLandingLead(code: string, payload: Partial<LeadCustomer>) {
  return apiPost<LeadCustomer>(`/api/landing/${code}/submit`, payload);
}
