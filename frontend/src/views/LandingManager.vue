<template>
  <section>
    <h2>引流页</h2>
    <el-card>
      <h3>{{ config?.title ?? '母婴月嫂获客表单' }}</h3>
      <p>来源码：{{ code }}</p>
      <p>表单字段：姓名、电话、微信、城市、意向产品、需求说明。</p>
      <el-form label-width="100px">
        <el-form-item label="姓名"><el-input v-model="form.name" /></el-form-item>
        <el-form-item label="电话"><el-input v-model="form.phone" /></el-form-item>
        <el-form-item label="微信"><el-input v-model="form.wechat" /></el-form-item>
        <el-form-item label="城市"><el-input v-model="form.city" /></el-form-item>
        <el-form-item label="需求"><el-input v-model="form.consultationContent" type="textarea" /></el-form-item>
        <el-button type="primary" @click="submit">模拟提交线索</el-button>
      </el-form>
      <el-alert v-if="createdLead" type="success" :closable="false" :title="`已入库：${createdLead.name}`" />
    </el-card>
  </section>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { getLandingConfig, submitLandingLead, type LandingConfig } from '../api/landing';
import type { LeadCustomer } from '../types/lead';

const code = 'maternity-cd-douyin-001';
const config = ref<LandingConfig>();
const createdLead = ref<LeadCustomer>();
const form = reactive({ name: '赵女士', phone: '13800000009', wechat: '', city: '成都', sourcePlatform: 'H5Form', interestedProduct: '月嫂', consultationContent: '预产期 10 月，想了解月嫂价格' });

onMounted(async () => { config.value = await getLandingConfig(code); });
async function submit() { createdLead.value = await submitLandingLead(code, form); }
</script>
