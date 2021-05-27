import React from 'react';
import axios from 'axios';
import {AuthContext} from '../AuthContext';
import { Redirect } from 'react-router';

class Logout extends React.Component {
    state = { 
      
     }

     componentDidMount =async () => {
         await axios.post('/api/account/logout');
     }     

    render() { 
       return <AuthContext.Consumer>
          {userdetails => {
           userdetails.logout();  
           return <Redirect to ='/login'/>                   
         }}
        </AuthContext.Consumer>        
    }
}
 
export default Logout;