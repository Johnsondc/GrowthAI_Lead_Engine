<template>
  <div>
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px">
      <h2 style="margin: 0">来源管理</h2>
      <el-button type="primary" @click="showCreate = true">新增来源</el-button>
    </div>
    <el-card>
      <el-table :data="sources" stripe v-loading="loading">
        <el-table-column prop="sourceType" label="来源类型" width="150" />
        <el-table-column prop="platform" label="平台" width="120" />
        <el-table-column prop="accountName" label="账号" />
        <el-table-column prop="trackingCode" label="追踪码" width="220" />
        <el-table-column prop="createdAt" label="创建时间" width="170" />
        <el-table-column label="操作" width="100">
          <template #default="{ row }">
            <el-button size="small" type="danger" @click="deleteSource(row.id)">停用</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
    <el-dialog v-model="showCreate" title="新增来源" width="400px">
      <el-form :model="newSource" label-width="80px">
        <el-form-item label="类型">
          <el-select v-model="newSource.sourceType">
            <el-option label="LandingForm" value="LandingForm" />
            <el-option label="PlatformSearch" value="PlatformSearch" />
            <el-option label="KeywordRecommend" value="KeywordRecommend" />
            <el-option label="ManualInput" value="ManualInput" />
          </el-select>
        </el-form-item>
        <el-form-item label="平台"><el-input v-model="newSource.platform" /></el-form-item>
        <el-form-item label="账号"><el-input v-model="newSource.accountName" /></el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showCreate = false">取消</el-button>
        <el-button type="primary" @click="createSource">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import api from '../api'
import { ElMessage } from 'element-plus'
const sources = ref([])
const loading = ref(false)
const showCreate = ref(false)
const newSource = reactive({ sourceType: 'LandingForm', platform: '', accountName: '' })
async function loadData() {
  loading.value = true
  try { const res = await api.get('/sources'); sources.value = res.data } catch (e) { console.error(e) }
  loading.value = false
}
async function createSource() {
  try { await api.post('/sources', newSource); ElMessage.success('创建成功'); showCreate.value = false; loadData() }
  catch (e: any) { ElMessage.error(e.response?.data?.message || '失败') }
}
async function deleteSource(id: number) {
  try { await api.delete(`/sources/${id}`); ElMessage.success('已停用'); loadData() }
  catch (e: any) { ElMessage.error('操作失败') }
}
onMounted(loadData)
</script>
