<template>
  <div class="hello">
    <h1>{{ props.msg }}</h1>
  </div>
  <div class="container">
    <div class="sidebar">
      <ul>
        <li>
          <a href="#" @click="spa.navigateToUrl('/app1')">App1</a>
        </li>
        <li>
          <a href="#" @click="spa.navigateToUrl('/app2')">App2</a>
        </li>
      </ul>
    </div>
    <div class="content">
      <div id="single-spa-application:@b-velop/app1"></div>
      <div id="single-spa-application:@b-velop/app2"></div>
    </div>
  </div>
</template>

<script setup lang="ts">
import * as spa from "single-spa";
import { Store } from "infra";
import { defineProps } from "vue";

const props = defineProps<{ msg: string; store: Store }>();
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="css">
.hello {
  display: flex;
  justify-content: center;
  background-color: #f0f0f0;
  font-size: 2em;
  font-family: Helvetica, sans-serif;
}

.container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 100vh;
}

.sidebar {
  background-color: #f0f0f0;
  height: 100%;
  margin-top: 4rem;
  padding: 1rem;
}

.content {
  background-color: azure;
  height: 100%;
  margin-top: 4rem;
  margin-left: 1rem;
  padding: 1rem;
  flex-grow: 1;
  border: 1px solid black;
}
</style>
