const {resolve} = require("node:path");

const project = resolve(process.cwd(), "tsconfig.json");

/** @type {import("eslint").Linter.Config} */
module.exports = {
    extends: [
        "eslint:recommended",
        "eslint-config-turbo",
        "plugin:prettier/recommended",
        'plugin:vue/vue3-essential',
        '@vue/standard',
        '@vue/typescript/recommended',
    ],
    env: {
        node: true,
        browser: true,
    },
    plugins: ["only-warn"],
    settings: {
        "import/resolver": {
            typescript: {
                project,
            },
        },
    },
    ignorePatterns: [
        // Ignore dotfiles
        ".*.js",
        "node_modules/",
    ],
    rules: {
        'semi': ['error', 'always'],
        "space-before-function-paren": ["error", "never"],
        'quotes': ['error', 'double'],
        'comma-dangle': ['error', 'always-multiline'],
        'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
        'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off'
    },
    overrides: [{files: ["*.js?(x)", "*.ts?(x)"]}],
};
