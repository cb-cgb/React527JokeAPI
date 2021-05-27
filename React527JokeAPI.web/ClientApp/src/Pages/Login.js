import React from 'react';
import axios from 'axios';
import {produce} from 'immer';
import {Link} from 'react-router-dom';
import {AuthContext} from '../AuthContext';

class Login extends React.Component {

    state =  {  
        user: {
            email: '',
            password: ''
        },
        isValidLogin:true
    }

    onTextChange = e => {
     const nextState = produce(this.state, draft => {
         draft.user[e.target.name] = e.target.value;          
     })    
      this.setState(nextState);
    }

    resetUser =() => {
        const email = '';
        const password = '';
        if(!this.state.isValidLogin) {
            this.setState({user: {email, password}});
        }
        
    }

     onSubmitForm  = async(e,setUser) => {
        e.preventDefault();
        const {data} =await axios.post('/api/account/login', this.state.user)
        const isValidLogin=!!data;
        this.setState({isValidLogin});       
        await setUser();
        isValidLogin &&  this.props.history.push('/');
        this.resetUser(); //resets the login form to blank if invalid user/pw
    }

    render() { 
          return(
            <AuthContext.Consumer>
                {userDetails => {
                    const {setUser} = userDetails;
                                     
                 const {isValidLogin} = this.state;
                 const {email, password} = this.state.user;
        return ( 
          
            <div className="row card card-body bg-light" style={{width:400,textAlign:"left"}}>

              <h3 style={{textAlign:"center"}}>Login</h3>
              {!isValidLogin && <h5 className="text-danger" >Invalid user/password. Please try again</h5>}
            
              <form onSubmit={e => this.onSubmitForm(e, setUser)}>
                <input value={email} type="text" className="form-control" name="email" onChange={this.onTextChange} placeholder="Enter your email"  />
                <input value={password} type="password" className="form-control" name="password" onChange={this.onTextChange}  placeholder="Enter your password" />
                <br/>
                <button className="btn btn-lg btn-info">Login</button>
             </form>
             <Link to="/signup">Don't have an account? Register here.</Link>
          </div> 
           

         );
          }}

        </AuthContext.Consumer>
    ) 
 
    }
}
 
export default Login;