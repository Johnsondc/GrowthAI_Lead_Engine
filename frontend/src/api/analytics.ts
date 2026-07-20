import { apiGet } from './client';
import type { OverviewMetrics } from '../types/lead';

export function getOverview() {
  return apiGet<OverviewMetrics>('/api/analytics/overview');
}
