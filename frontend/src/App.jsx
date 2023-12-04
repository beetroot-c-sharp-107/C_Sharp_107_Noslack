import React, { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

// JSX Element
function Message({ avatarUrl, nickname, lastMessage }) {
  return (
    <div>
      <div>Avatar</div>
      <div>Nickname</div>
      <div>Last Message</div>
    </div>
  )
}

function App() {
  const [messages, setMessages] = React.useState([]);

  React.useEffect(() => {
    const onScroll = (e) => {
      // DOM – Document Object Model
      // Virtual DOM – Virtual Document Object Model
      const onTop = true;
      if (onTop) {
        fetch(`/api/chat/${1}/messages?start=${0}&count=${10}`)
          .then(r => r.json())
          .then(r => setMessages(m => [...m, ...r]))
          .catch(e => console.error(e));
      }
    };
    window.addEventListener("scroll", onScroll);
    return () => window.removeEventListener("scroll", onScroll);
  }, []);

  return (
    <>{
      messages && messages.map(m => <Message avatarUrl={m.avatarUrl} nickname={m.nickname} lastMessage={m.lastMessage} />)
    }</>
  )
}

export default App
