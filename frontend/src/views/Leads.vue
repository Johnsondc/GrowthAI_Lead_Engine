<template>
  <div>
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px">
      <h2 style="margin: 0">客户池</h2>
      <el-button type="primary" @click="showCreate = true">新增客户</el-button>
    </div>
    <el-card>
      <el-form :inline="true" :model="query" style="margin-bottom: 16px">
        <el-form-item>
          <el-input v-model="query.keyword" placeholder="搜索姓名/手机/微信" clearable style="width: 200px" />
        </el-form-item>
        <el-form-item>
          <el-select v-model="query.status" placeholder="状态" clearable style="width: 120px">
            <el-option label="New" value="New" />
            <el-option label="Contacted" value="Contacted" />
            <el-option label="InProgress" value="InProgress" />
            <el-option label="Hot" value="Hot" />
            <el-option label="Closed" value="Closed" />
            <el-option label="Invalid" value="Invalid" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="loadData">查询</el-button>
        </el-form-item>
      </el-form>
      <el-table :data="leads" stripe v-loading="loading">
        <el-table-column prop="name" label="姓名" width="100" />
        <el-table-column prop="phone" label="手机号" width="130" />
        <el-table-column prop="weChat" label="微信" width="120" />
        <el-table-column prop="city" label="城市" width="80" />
        <el-table-column prop="sourcePlatform" label="来源平台" width="100" />
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="statusColor(row.status)" size="small">{{ row.status }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="assignedUserName" label="负责人" width="80" />
        <el-table-column prop="createdAt" label="创建时间" width="170" />
      </el-table>
      <el-pagination style="margin-top: 16px; text-align: right"
        :current-page="query.page" :page-size="query.pageSize" :total="total"
        layout="total, prev, pager, next" @current-change="(p: number) => { query.page = p; loadData() }" />
    </el-card>

    <el-dialog v-model="showCreate" title="新增客户" width="500px">
      <el-form :model="newLead" label-width="80px">
        <el-form-item label="姓名"><el-input v-model="newLead.name" /></el-form-item>
        <el-form-item label="手机号"><el-input v-model="newLead.phone" /></el-form-item>
        <el-form-item label="微信"><el-input v-model="newLead.weChat" /></el-form-item>
        <el-form-item label="城市"><el-input v-model="newLead.city" /></el-form-item>
        <el-form-item label="咨询内容"><el-input v-model="newLead.consultContent" type="textarea" /></el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showCreate = false">取消</el-button>
        <el-button type="primary" @click="createLead">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import api from '../api'
import { ElMessage } from 'element-plus'

const leads = ref([])
const loading = ref(false)
const total = ref(0)
const showCreate = ref(false)
const query = reactive({ keyword: '', status: '', page: 1, pageSize: 20 })
const newLead = reactive({ name: '', phone: '', weChat: '', city: '', consultContent: '' })

function statusColor(s: string) {
  const map: Record<string, string> = { New: 'info', Contacted: 'warning', InProgress: 'primary', Hot: 'danger', Closed: 'success', Invalid: '' }
  return map[s] || 'info'
}

async function loadData() {
  loading.value = true
  try {
    const res = await api.get('/leads', { params: query })
    leads.value = res.data.items
    total.value = res.data.total
  } catch (e) { console.error(e) }
  loading.value = false
}

async function createLead() {
  try {
    await api.post('/leads', newLead)
    ElMessage.success('创建成功')
    showCreate.value = false
    loadData()
  } catch (e: any) {
    ElMessage.error(e.response?.data?.message || '创建失败')
  }
}

onMounted(loadData)
</script>
