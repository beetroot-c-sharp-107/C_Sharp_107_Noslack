import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

import Chatbox from './Chatbox'

function App() {
  const chats = [
    {
      chatId: 1,
      title: "Messages with Illia",
      text: "Hey! Sup you, how its going..."
    },
    {
      chatId: 2,
      title: "Messages with Yana",
      text: "Hi! How is your homework?"
    },
    {
      chatId: 3,
      title: "Messages with Yuri",
      text: "Hi! How is your homework?"
    },
    {
      chatId: 4,
      title: "Messages with Natali",
      text: "Hi! How is your homework?"
    },
    {
      chatId: 7,
      title: "Messages with Zorian",
      text: "Hi! How is your homework?"
    }
  ]

  return (
    <div className="chatlist">
      {
        chats.map(chat => <Chatbox chatId={chat.chatId} title={chat.title} text={chat.text} />)
      }
    </div>
  )
}

export default App
