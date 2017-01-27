import {createStore, applyMiddleware, compose} from 'redux'
import thunk from 'redux-thunk';
import reducers from './reducers'

const getEnhancers = () => {
    let enhancers = [];
    if(window.devToolsExtension){
        enhancers.push(window.devToolsExtension());
    }
    return enhancers;
};

export const configureStore = (initialState) => {
    return createStore(reducers, compose(applyMiddleware(thunk), ...getEnhancers()));
};