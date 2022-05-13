import React from 'react';
import { Menu } from 'antd';
import { Link } from 'react-router-dom'

export default function Menucomponent({routes}){

    
  
    return <Menu mode="inline" theme="light" 
    defaultSelectedKeys='/'
    > 
        {routes.map(function(route){
           return   <Menu.Item  key={route.path}>
             {route.title}
               <Link to={route.path} />
           </Menu.Item>
        })}
        
    </Menu>
}