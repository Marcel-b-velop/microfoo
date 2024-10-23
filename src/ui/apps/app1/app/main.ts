import { h, createApp } from "vue";
import singleSpaVue from "single-spa-vue";
import PrimeVue from "primevue/config";
import Button from "primevue/button";
import "primeicons/primeicons.css";
import Aura from "@primevue/themes/aura";

import App from "./App.vue";

const vueLifecycles = singleSpaVue({
  createApp,
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  // @ts-ignore
  payload: {
    store: undefined,
  },
  appOptions: {
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-ignore
    render() {
      // eslint-disable-next-line @typescript-eslint/ban-ts-comment
      // @ts-ignore
      return h(App, {
        // eslint-disable-next-line @typescript-eslint/ban-ts-comment
        // @ts-ignore
        store: this.payload.store,
        // single-spa props are available on the "this" object. Forward them to your component as needed.
        // https://single-spa.js.org/docs/building-applications#lifecycle-props
        // if you uncomment these, remember to add matching prop definitions for them in your App.vue file.
        /*
        name: this.name,
        mountParcel: this.mountParcel,
        singleSpa: this.singleSpa,
        */
      });
    },
  },
  handleInstance(app) {
    app.use(PrimeVue, {
      theme: {
        preset: Aura,
      },
    });
    app.component("Button", Button);
  },
  replaceMode: false,
});

export const bootstrap = vueLifecycles.bootstrap;
export const mount = vueLifecycles.mount;
export const unmount = vueLifecycles.unmount;
