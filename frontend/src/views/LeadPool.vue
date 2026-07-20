<template>
  <section>
    <h2>客户池</h2>
    <el-table v-loading="store.loading" :data="store.leads" border>
      <el-table-column prop="name" label="客户姓名" />
      <el-table-column prop="phone" label="手机号" />
      <el-table-column prop="wechat" label="微信" />
      <el-table-column prop="city" label="城市" />
      <el-table-column prop="sourcePlatform" label="来源平台" />
      <el-table-column prop="interestedProduct" label="意向产品" />
      <el-table-column prop="status" label="状态" />
      <el-table-column prop="consultationContent" label="咨询内容" />
    </el-table>
  </section>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useLeadStore } from '../stores/leadStore';

const store = useLeadStore();
onMounted(() => store.load());
</script>
