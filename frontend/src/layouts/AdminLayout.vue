<template>
  <el-container style="height: 100vh">
    <el-aside width="220px" style="background: #1d1e1f">
      <div style="padding: 20px; color: #fff; font-size: 18px; font-weight: bold; text-align: center">
        GrowthAI
      </div>
      <el-menu :default-active="route.path" router background-color="#1d1e1f" text-color="#bfcbd9" active-text-color="#409EFF">
        <el-menu-item index="/dashboard">
          <el-icon><DataLine /></el-icon>
          <span>数据看板</span>
        </el-menu-item>
        <el-menu-item index="/leads">
          <el-icon><User /></el-icon>
          <span>客户池</span>
        </el-menu-item>
        <el-menu-item index="/contents">
          <el-icon><Document /></el-icon>
          <span>内容中心</span>
        </el-menu-item>
        <el-menu-item index="/sources">
          <el-icon><Link /></el-icon>
          <span>来源管理</span>
        </el-menu-item>
        <el-menu-item index="/landing-pages">
          <el-icon><Monitor /></el-icon>
          <span>引流页</span>
        </el-menu-item>
        <el-menu-item index="/settings">
          <el-icon><Setting /></el-icon>
          <span>系统设置</span>
        </el-menu-item>
      </el-menu>
    </el-aside>
    <el-container>
      <el-header style="display: flex; align-items: center; justify-content: flex-end; border-bottom: 1px solid #eee">
        <span style="margin-right: 16px">{{ auth.userName }} ({{ auth.role }})</span>
        <el-button type="danger" size="small" @click="handleLogout">退出</el-button>
      </el-header>
      <el-main style="background: #f5f7fa; overflow-y: auto">
        <router-view />
      </el-main>
    </el-container>
  </el-container>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { DataLine, User, Document, Link, Monitor, Setting } from '@element-plus/icons-vue'

const route = useRoute()
const router = useRouter()
const auth = useAuthStore()

function handleLogout() {
  auth.logout()
  router.push('/login')
}
</script>
