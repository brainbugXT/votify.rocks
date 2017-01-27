import React from 'react';
import ReactDOM, {render} from 'react-dom';
import VoteApp from './components/component-app-main'
import {Provider} from 'react-redux'
import {configureStore} from './store'
import injectTapEventPlugin from 'react-tap-event-plugin';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';

const store = configureStore();
injectTapEventPlugin();
render(
    <Provider store={store}>
        <MuiThemeProvider>
            <VoteApp />
        </MuiThemeProvider>
    </Provider>, 
    document.getElementById('app'));
