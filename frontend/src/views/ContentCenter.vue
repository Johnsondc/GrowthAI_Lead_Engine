<template>
  <section>
    <h2>内容中心</h2>
    <el-card>
      <el-form label-width="100px">
        <el-form-item label="行业"><el-input v-model="form.industry" /></el-form-item>
        <el-form-item label="产品"><el-input v-model="form.productName" /></el-form-item>
        <el-form-item label="城市"><el-input v-model="form.city" /></el-form-item>
        <el-form-item label="目标客户"><el-input v-model="form.targetAudience" /></el-form-item>
        <el-form-item label="卖点"><el-input v-model="form.sellingPoints" type="textarea" /></el-form-item>
        <el-button type="primary" @click="submit">生成获客内容</el-button>
      </el-form>
    </el-card>
    <el-card v-if="result" class="result-card">
      <h3>{{ result.title }}</h3>
      <p>{{ result.body }}</p>
      <pre>{{ result.script }}</pre>
      <p>{{ result.tags }}</p>
      <strong>{{ result.callToAction }}</strong>
    </el-card>
  </section>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import { generateContent, type GeneratedContent } from '../api/aiContent';

const form = reactive({ industry: '母婴', productName: '月嫂', city: '成都', targetAudience: '孕妇家庭', sellingPoints: '本地阿姨、严格筛选、快速匹配', platform: 'Douyin' });
const result = ref<GeneratedContent>();
async function submit() { result.value = await generateContent(form); }
</script>
