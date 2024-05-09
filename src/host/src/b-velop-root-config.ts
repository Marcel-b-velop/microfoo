import { registerApplication, start, LifeCycles } from "single-spa";

registerApplication({
  name: "@single-spa/welcome",
  app: () =>
    System.import<LifeCycles>(
      "https://unpkg.com/single-spa-welcome/dist/single-spa-welcome.js"
    ),
  activeWhen: ["/"],
});

// registerApplication({
//   name: "@b-velop/navbar",
//   app: () => System.import("@b-velop/navbar"),
//   activeWhen: ["/"]
// });

start({
  urlRerouteOnly: true,
});
