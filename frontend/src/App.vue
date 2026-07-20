<template>
  <el-container class="app-shell">
    <el-aside width="248px" class="sidebar">
      <h1>GrowthAI</h1>
      <p>AI 新媒体获客</p>
      <el-menu router default-active="/dashboard">
        <el-menu-item index="/dashboard">数据总览</el-menu-item>
        <el-menu-item index="/leads">客户池</el-menu-item>
        <el-menu-item index="/content">内容中心</el-menu-item>
        <el-menu-item index="/landing">引流页</el-menu-item>
        <el-menu-item index="/analytics">数据分析</el-menu-item>
        <el-menu-item index="/settings">企业设置</el-menu-item>
      </el-menu>
    </el-aside>
    <el-main>
      <router-view />
    </el-main>
  </el-container>
</template>
