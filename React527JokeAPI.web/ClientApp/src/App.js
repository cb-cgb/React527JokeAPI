import React, { Component } from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './Pages/Home';
import Signup from './Pages/Signup';
import Login from './Pages/Login';
import Logout from './Pages/Logout';
import All from './Pages/All';

import {AuthContextComponent} from './AuthContext';



export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
     <AuthContextComponent>
      <Layout>
        <Route exact path='/' component={Home}/>
        <Route exact path='/signup' component = {Signup}/>
        <Route exact path='/login' component = {Login}/>
        <Route exact path='/logout' component = {Logout}/>
        <Route exact path='/alljokes' component = {All}/>
        
      </Layout>
      </AuthContextComponent>
    );
  }
}
