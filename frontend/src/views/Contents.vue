<template>
  <div>
    <h2>内容中心</h2>
    <el-card>
      <el-table :data="contents" stripe v-loading="loading">
        <el-table-column prop="title" label="标题" />
        <el-table-column prop="targetPlatform" label="平台" width="100" />
        <el-table-column prop="contentType" label="类型" width="120" />
        <el-table-column prop="createdAt" label="创建时间" width="170" />
      </el-table>
      <el-pagination style="margin-top: 16px; text-align: right"
        :current-page="page" :page-size="20" :total="total"
        layout="total, prev, pager, next" @current-change="(p: number) => { page = p; loadData() }" />
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../api'
const contents = ref([])
const loading = ref(false)
const page = ref(1)
const total = ref(0)
async function loadData() {
  loading.value = true
  try {
    const res = await api.get('/contents', { params: { page: page.value, pageSize: 20 } })
    contents.value = res.data.items
    total.value = res.data.total
  } catch (e) { console.error(e) }
  loading.value = false
}
onMounted(loadData)
</script>
