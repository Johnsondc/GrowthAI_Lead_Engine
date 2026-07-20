import { apiGet, apiPost } from './client';
import type { LeadCustomer, LeadStatus, SourcePlatform } from '../types/lead';

export function listLeads(params: { status?: LeadStatus; platform?: SourcePlatform } = {}) {
  const query = new URLSearchParams();
  if (params.status) query.set('status', params.status);
  if (params.platform) query.set('platform', params.platform);
  const suffix = query.toString() ? `?${query}` : '';
  return apiGet<LeadCustomer[]>(`/api/leads${suffix}`);
}

export function createLead(payload: Partial<LeadCustomer>) {
  return apiPost<LeadCustomer>('/api/leads', payload);
}
