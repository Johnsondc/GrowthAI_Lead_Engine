<template>
  <div>
    <h2>系统设置</h2>
    <el-row :gutter="20">
      <el-col :span="12">
        <el-card>
          <template #header><span>企业信息</span></template>
          <el-descriptions :column="1" border>
            <el-descriptions-item label="企业名称">{{ enterprise.name }}</el-descriptions-item>
            <el-descriptions-item label="行业">{{ enterprise.industry }}</el-descriptions-item>
            <el-descriptions-item label="联系电话">{{ enterprise.contactPhone }}</el-descriptions-item>
            <el-descriptions-item label="套餐">{{ enterprise.planType }}</el-descriptions-item>
          </el-descriptions>
        </el-card>
      </el-col>
      <el-col :span="12">
        <el-card>
          <template #header><span>员工管理</span></template>
          <el-table :data="users" stripe size="small">
            <el-table-column prop="name" label="姓名" width="100" />
            <el-table-column prop="phone" label="手机号" width="130" />
            <el-table-column prop="role" label="角色" width="100" />
          </el-table>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import api from '../api'
const enterprise = reactive({ name: '', industry: '', contactPhone: '', planType: '' })
const users = ref([])
onMounted(async () => {
  try {
    const [ent, usr] = await Promise.all([api.get('/settings/enterprise'), api.get('/settings/users')])
    Object.assign(enterprise, ent.data)
    users.value = usr.data
  } catch (e) { console.error(e) }
})
</script>
