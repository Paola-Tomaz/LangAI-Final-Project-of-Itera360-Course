const NodePolyfillPlugin = require('node-polyfill-webpack-plugin');
const webpack = require('webpack');
const path = require('path');

module.exports = {
  webpack: {
    plugins: [
      new NodePolyfillPlugin({
        excludeAliases: ["console"]
      }),
      new webpack.ProvidePlugin({
        process: 'process/browser.js',
        Buffer: ['buffer', 'Buffer'],
      }),
    ],
    configure: (webpackConfig) => {
      webpackConfig.resolve = {
        ...webpackConfig.resolve,
        fallback: {
          ...webpackConfig.resolve.fallback,
          "fs": false,
          "path": require.resolve("path-browserify"),
          "stream": require.resolve("stream-browserify"),
          "crypto": require.resolve("crypto-browserify"),
          "http": require.resolve("stream-http"),
          "https": require.resolve("https-browserify"),
          "os": require.resolve("os-browserify/browser"),
          "process": require.resolve("process/browser"),
          "buffer": require.resolve("buffer/")
        },
        alias: {
          ...webpackConfig.resolve.alias,
          'process/browser': path.resolve(__dirname, 'node_modules/process/browser.js')
        }
      };
      
      webpackConfig.ignoreWarnings = [/Failed to parse source map/];
      webpackConfig.module.rules.push({
        test: /\.m?js$/,
        resolve: {
          fullySpecified: false
        }
      });
      
      return webpackConfig;
    }
  }
};