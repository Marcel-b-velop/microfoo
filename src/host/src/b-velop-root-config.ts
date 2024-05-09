import { registerApplication, start, LifeCycles } from "single-spa";

registerApplication({
  name: "@b-velop/frame",
  app: () => System.import<LifeCycles>("@b-velop/frame"),
  activeWhen: ["/"],
});

start({
  urlRerouteOnly: true,
});

registerApplication({
  name: "@b-velop/app1",
  app: () => System.import<LifeCycles>("@b-velop/app1"),
  activeWhen: (location: Location) =>
    location.pathname.startsWith("/app1") || location.pathname === "/",
});

registerApplication({
  name: "@b-velop/app2",
  app: () => System.import<LifeCycles>("@b-velop/app2"),
  activeWhen: (location: Location) => location.pathname.startsWith("/app2"),
});
