import { defineStore } from 'pinia';
import { listLeads } from '../api/leads';
import type { LeadCustomer } from '../types/lead';

export const useLeadStore = defineStore('leadStore', {
  state: () => ({
    leads: [] as LeadCustomer[],
    loading: false
  }),
  actions: {
    async load() {
      this.loading = true;
      try {
        this.leads = await listLeads();
      } finally {
        this.loading = false;
      }
    }
  }
});
