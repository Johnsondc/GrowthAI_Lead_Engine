<template>
  <div>
    <h2>引流页管理</h2>
    <el-card>
      <el-table :data="pages" stripe v-loading="loading">
        <el-table-column prop="title" label="标题" />
        <el-table-column prop="pageCode" label="页面编码" width="160" />
        <el-table-column prop="viewCount" label="浏览量" width="80" />
        <el-table-column prop="submitCount" label="提交量" width="80" />
        <el-table-column label="转化率" width="80">
          <template #default="{ row }">{{ row.viewCount > 0 ? (row.submitCount / row.viewCount * 100).toFixed(1) : 0 }}%</template>
        </el-table-column>
        <el-table-column prop="createdAt" label="创建时间" width="170" />
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../api'
const pages = ref([])
const loading = ref(false)
async function loadData() {
  loading.value = true
  try { const res = await api.get('/landing-pages'); pages.value = res.data } catch (e) { console.error(e) }
  loading.value = false
}
onMounted(loadData)
</script>
