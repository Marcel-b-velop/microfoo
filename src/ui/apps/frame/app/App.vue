<template>
  <HelloWorld :msg="msg" :store="props.store" />

</template>

<script setup lang="ts">
import HelloWorld from "./components/HelloWorld.vue";
import { onMounted, ref } from "vue";
import { Store, State } from "@repo/infra/src";

const letNavigate = ref(false);
const props = defineProps<{ store: Store }>();
const msg = ref("Welcome to Your Vue.js + TypeScript App");

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
      msg.value = data.message;
    },
  });
  st.next({ message: "Hello from Vue", shouldNavigate, token: "1234" });
});
</script>

<style lang="scss"></style>
