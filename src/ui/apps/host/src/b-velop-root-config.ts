import { getShouldNavigate, getStore, State } from "@repo/infra/src";
import { registerApplication, start, LifeCycles } from "single-spa";
import axios from "axios";

const store = getStore();
const shouldNavigate = getShouldNavigate();
const client = axios.create({
  baseURL: "//localhost:8000",
});

registerApplication({
  name: "@b-velop/frame",
  app: () => System.import<LifeCycles>("@b-velop/frame"),
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  activeWhen: (location: Location) => true,
  customProps: {
    payload: { store, shouldNavigate },
  },
});

client.get<LocationMap>("/api/Host/locationmap").then((res) => {
  const locationMap = res.data;

  store.subscribe((state: State) => {
    const navFunction = state.shouldNavigate;
    if (navFunction) {
      navFunction("doLogin Called from Host");
    }
  });

  start({
    urlRerouteOnly: true,
  });

  for (const [key, value] of Object.entries(locationMap.locations)) {
    console.log(`Key: ${key}, Value: ${value}`);
    registerApplication({
      name: key,
      app: () => System.import<LifeCycles>(key),
      activeWhen: (location: Location) => location.pathname.startsWith(value),
      customProps: {
        payload: { store, shouldNavigate },
      },
    });
  }

  registerApplication({
    name: "@b-velop/app2",
    app: () => System.import<LifeCycles>("@b-velop/app2"),
    activeWhen: (location: Location) => location.pathname.startsWith("/app2"),
    customProps: {
      payload: { store, shouldNavigate },
    },
  });

  setTimeout(() => {
    console.log("Hallo from host");
    store.next({
      message: "Testnachricht",
      token: "1234",
      shouldNavigate: (sender: string) => {
        console.log("shouldNavigate called from host", sender);
        return true;
      },
    });
  }, 5_000);
});
