<template>
  <div>
    <h2 style="margin-bottom: 20px">数据看板</h2>
    <el-row :gutter="20" style="margin-bottom: 20px">
      <el-col :span="6" v-for="item in cards" :key="item.label">
        <el-card shadow="hover">
          <div style="text-align: center">
            <div style="font-size: 32px; font-weight: bold; color: #409EFF">{{ item.value }}</div>
            <div style="color: #999; margin-top: 8px">{{ item.label }}</div>
          </div>
        </el-card>
      </el-col>
    </el-row>
    <el-card>
      <template #header><span>客户来源分布</span></template>
      <el-table :data="sources" stripe>
        <el-table-column prop="sourceType" label="来源类型" />
        <el-table-column prop="count" label="数量" />
        <el-table-column prop="percentage" label="占比(%)">
          <template #default="{ row }">{{ row.percentage }}%</template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../api'

const cards = ref([
  { label: '客户总数', value: 0 },
  { label: '今日新增', value: 0 },
  { label: '热门客户', value: 0 },
  { label: '内容总数', value: 0 },
])
const sources = ref([])

onMounted(async () => {
  try {
    const [overview, dist] = await Promise.all([
      api.get('/dashboard/overview'),
      api.get('/dashboard/source-distribution')
    ])
    cards.value = [
      { label: '客户总数', value: overview.data.totalLeads },
      { label: '今日新增', value: overview.data.newLeadsToday },
      { label: '热门客户', value: overview.data.hotLeads },
      { label: '内容总数', value: overview.data.totalContents },
    ]
    sources.value = dist.data
  } catch (e) { console.error(e) }
})
</script>
