const {join, resolve} = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const webpack = require('webpack');
const root = resolve(__dirname, '..');

module.exports = (env) => {

  const ifDev = (what) => env.dev === true ? what : undefined;
  const ifProd = (what) => env.prod === true ? what : undefined;
  const removeUndefined = (arr) => arr.filter(x => x !== undefined);

  return {
    entry: {
      main: './index.jsx',
    },
    output: {
      path: join(root, 'dist'),
      filename: '[name].bundle.js',
      sourceMapFilename: '[file].map',
      pathinfo: true,
    },
    context: join(root, 'src'),
    resolve: {
      extensions: ['', '.js', '.json', '.jsx'],
    },
    module: {
      loaders: [
        {
          test: /\.js(x?)$/,
          loader: 'babel-loader',
          exclude: [ /node_modules/]
        },
        {
          test: /\.css$/,
          loaders: [
            'style-loader',
            'css-loader',
          ]
        },
        {
          test: /\.scss$/,
          loaders: ["style", "css", "sass"]
        }
      ]
    },
    plugins: removeUndefined([
      new HtmlWebpackPlugin({
        template: './index.html'
      }),
      ifProd(
        new webpack.DefinePlugin({
          'process.env': {
            NODE_ENV: JSON.stringify('production')
          }
        })
      ),
      ifProd(
        new webpack.optimize.UglifyJsPlugin({
          compress: {
            screw_ie8: true
          },
          sourceMap: false
        })
      ),
    ]),
  };

};
