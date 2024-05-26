import { getShouldNavigate, getStore, State } from "@repo/infra/src";
import { registerApplication, start, LifeCycles } from "single-spa";

const store = getStore();
const shouldNavigate = getShouldNavigate();

registerApplication({
  name: "@b-velop/frame",
  app: () => System.import<LifeCycles>("@b-velop/frame"),
  activeWhen: (location: Location) => true,
  customProps: {
    payload: { store, shouldNavigate }
  }
});

store.subscribe((state: State) => {
  const navFunction = state.shouldNavigate;
  if (navFunction) {
    navFunction("doLogin Called from Host");
  }
});

start({
  urlRerouteOnly: true
});

registerApplication({
  name: "@b-velop/app1",
  app: () => System.import<LifeCycles>("@b-velop/app1"),
  activeWhen: (location: Location) =>
    location.pathname.startsWith("/app1") || location.pathname === "/",
  customProps: {
    payload: { store, shouldNavigate }
  }
});

registerApplication({
  name: "@b-velop/app2",
  app: () => System.import<LifeCycles>("@b-velop/app2"),
  activeWhen: (location: Location) => location.pathname.startsWith("/app2"),
  customProps: {
    payload: { store, shouldNavigate }
  }
});

setTimeout(() => {
  console.log("Hallo from host");
  store.next({
    message: "Testnachricht",
    token: "1234",
    shouldNavigate: (sender: string) => {
      console.log("shouldNavigate called from host", sender);
      return true;
    }
  });
}, 5_000);
