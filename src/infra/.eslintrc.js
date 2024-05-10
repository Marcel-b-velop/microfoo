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
    parser: "@babel/eslint-parser",
    parserOptions: {
        ecmaVersion: 2020
    },
    rules: {
        'semi': ['error', 'never'],
        'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
        'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off'
    }
}
