<template>
  <section>
    <h2>数据分析</h2>
    <el-row :gutter="16">
      <el-col :span="12">
        <el-card>
          <h3>来源平台</h3>
          <p v-for="item in sources" :key="item.platform">{{ item.platform }}：{{ item.count }}</p>
        </el-card>
      </el-col>
      <el-col :span="12">
        <el-card>
          <h3>客户状态漏斗</h3>
          <p v-for="item in funnel" :key="item.status">{{ item.status }}：{{ item.count }}</p>
        </el-card>
      </el-col>
    </el-row>
  </section>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { getFunnelMetrics, getSourceMetrics, type DimensionMetric } from '../api/analytics';

const sources = ref<DimensionMetric[]>([]);
const funnel = ref<DimensionMetric[]>([]);

onMounted(async () => {
  sources.value = await getSourceMetrics();
  funnel.value = await getFunnelMetrics();
});
</script>
