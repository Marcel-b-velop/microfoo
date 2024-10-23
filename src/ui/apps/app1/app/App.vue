<template>
  <HelloWorld msg="Welcome to App1" />
  <label>{{ letNavigate }}</label>
  <button @click="letNavigate = !letNavigate">Toggle Navigation</button>
  <Button class="ml-2" @click="openApp">Open App</Button>
  <br />
  <label>{{ closed ? "Window Closed" : "" }}</label>
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
const closed = ref<boolean>(false);
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

const openApp = () => {
  closed.value = false;
  console.info("openApp");
  letNavigate.value = true;
  const newWindow = window.open("http://localhost:5173", "_blank", "width=400,height=600,top=100,left=100, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no");
  if (!newWindow) {
    return;
  }

  // newWindow.addEventListener("message", (event) => {
  //   console.info("message", event.data);
  // });
  window.addEventListener("message", (event) => {
    if (event.origin !== "http://localhost:5173") {
      return;
    }
    console.info("message", event.origin, event.data);
  });

  newWindow.onload = () => {};

  newWindow.postMessage(
    "Host Call Pppp: Hello from Vue",
    "http://localhost:5173",
  );

  const checkWindowClosed = setInterval(() => {
    if (newWindow.closed) {
      console.info("New window is closed");
      closed.value = true;
      clearInterval(checkWindowClosed);
    }
    newWindow.postMessage("Hello from Host", "http://localhost:5173");
  }, 1000); // Check every second
};
</script>

<style></style>
