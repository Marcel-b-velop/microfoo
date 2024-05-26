const { defineConfig } = require('@vue/cli-service')
const webpack = require('webpack')

module.exports = defineConfig({
  outputDir: 'dist',
  devServer: {
    historyApiFallback: true,
    open: false,
    hot: false,
    port: 9001,
    host: 'localhost',
    headers: {
      'Access-Control-Allow-Origin': '*'
    },
    allowedHosts: ['localhost', 'b-velop.com']
  },

  chainWebpack: (config) => {
    config
      .entry('app')
      .clear()
      .add('./src/main.ts')
      .end()
      .output.libraryTarget('system')
      .filename('b-velop-frame.js')
      .end()

    config.optimization.delete('splitChunks').end()

    config
      .plugin('LimitChunkCountPlugin')
      .use(webpack.optimize.LimitChunkCountPlugin, [{ maxChunks: 1 }])

    config.plugin('SystemJSPublicPathWebpackPlugin').tap((args) => {
      args[0].systemjsModuleName = 'b-velop-frame'
      args[0].rootDirectoryLevel = 1
      return args
    })

    config.module
      .rule('ts')
      .parser({ system: true })
      .rule('scss')
      .test(/\.scss$/)
      .use('style-loader')
      .loader('style-loader')
      .end()
      .use('css-loader')
      .loader('css-loader')
      .end()
      .use('sass-loader')
      .loader('sass-loader')
      .end()
  },
  transpileDependencies: true
})
