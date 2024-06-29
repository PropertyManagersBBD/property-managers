import React, { useEffect,useState } from "react";
import "./style.css"
import SideBar from "../sidebar";
import { useLocation } from 'react-router-dom';

const Layout = ({ children }) => {
    const [selected, setSelectedButton] =useState("/");
    const location=useLocation();
    useEffect(()=>{
        console.log("RAN")
        setSelectedButton(location.pathname);
    },[location.pathname])
    return (
      <section className="fullBody">
        <article className="sidebarWrapper">

        <SideBar Selected={selected}/>
        </article>
        <main className="mainClass">{children}</main>
      </section>
    );
  };


export default Layout;