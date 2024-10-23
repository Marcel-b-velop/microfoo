<template>
  <HelloWorld msg="Welcome to App1" />
  <label>{{ letNavigate }}</label>
  <button @click="letNavigate = !letNavigate">Toggle Navigation</button>
  <Lala></Lala>
</template>

<script setup lang="ts">
import { State, Store } from "@repo/infra/src";
import HelloWorld from "./components/HelloWorld.vue";
import { onMounted, ref, defineProps } from "vue";

const props = defineProps<{ store: Store }>();
const letNavigate = ref(false);

const shouldNavigate = (sender: string) => {
  console.info(sender);
  return letNavigate.value;
};

onMounted(() => {
  console.info("mounted");
  const st = props.store;
  st.subscribe({
    next: (data: State) => {
      console.info("data", data);
    },
  });
  st.next({ message: "Hello from Vue", shouldNavigate, token: "1234" });
});
</script>

<style></style>
