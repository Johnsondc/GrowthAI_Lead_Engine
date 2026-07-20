<template>
  <section>
    <h2>数据总览</h2>
    <el-row :gutter="16">
      <el-col :span="6" v-for="card in cards" :key="card.label">
        <el-card><span>{{ card.label }}</span><strong>{{ card.value }}</strong></el-card>
      </el-col>
    </el-row>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { getOverview } from '../api/analytics';
import type { OverviewMetrics } from '../types/lead';

const metrics = ref<OverviewMetrics>({ todayNewLeads: 0, validLeads: 0, highIntentLeads: 0, wonLeads: 0 });
const cards = computed(() => [
  { label: '今日新增客户', value: metrics.value.todayNewLeads },
  { label: '有效客户', value: metrics.value.validLeads },
  { label: '高意向客户', value: metrics.value.highIntentLeads },
  { label: '成交客户', value: metrics.value.wonLeads }
]);

onMounted(async () => {
  metrics.value = await getOverview();
});
</script>
