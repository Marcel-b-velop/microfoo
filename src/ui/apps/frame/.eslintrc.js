/** @type {import("eslint").Linter.Config} */
module.exports = {
  root: true,
  extends: ["@repo/eslint-config/vue.js"],
  parserOptions: {
    // project: true,
    project: "./tsconfig.json",
  },
};
