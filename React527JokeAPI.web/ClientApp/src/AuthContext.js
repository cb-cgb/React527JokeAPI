import React from 'react';  
import axios from 'axios';

const AuthContext = React.createContext();

class AuthContextComponent extends React.Component {
    state = { 
        user:null
     }

    componentDidMount= async()=> {
        this.setCurrentUser();
    }


     setCurrentUser =async()=> {
       const {data} = await axios.get('/api/account/getuser');
       this.setState({user: data});       
     } 

     logout = () =>  {
         this.setState({user: null});
     }

    render() { 
        const obj = {
              user:this.state.user,
              setUser:this.setCurrentUser,
              logout: this.logout,
            }
            return (
                <AuthContext.Provider value={obj}>
                    {this.props.children}
                </AuthContext.Provider>

            )
    }
}
 
export { AuthContextComponent, AuthContext}