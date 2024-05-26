module.exports = {
  root: true,
  env: {
    node: true
  },
  extends: [
    "plugin:prettier/recommended",
    'plugin:vue/vue3-essential',
    '@vue/standard',
    '@vue/typescript/recommended',
  ],
  parser: "vue-eslint-parser",
  parserOptions: {
    parser: "@typescript-eslint/parser",
    extraFileExtensions: ['.vue'],
    ecmaVersion: 2020
  },
  rules: {
    'semi': ['error', 'always'],
    'quotes': ['error', 'double'],
    'comma-dangle': ['error', 'always-multiline'],
    'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off'
  }
}
