import { apiGet } from './client';
import type { OverviewMetrics } from '../types/lead';

export interface DimensionMetric {
  platform?: string;
  status?: string;
  count: number;
}

export function getOverview() {
  return apiGet<OverviewMetrics>('/api/analytics/overview');
}

export function getSourceMetrics() {
  return apiGet<DimensionMetric[]>('/api/analytics/sources');
}

export function getFunnelMetrics() {
  return apiGet<DimensionMetric[]>('/api/analytics/funnel');
}
