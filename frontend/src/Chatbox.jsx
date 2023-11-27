
import './Chatbox.css'

function Chatbox(props) {
  return (
    <a href={`/chats/${props.chatId}`}>
      <div className="chatbox">
        <div className="avatar">
        </div>
        <div className="chatinfo">
          <h2>{props.title}</h2>
          <span>{props.text}</span>
        </div>
      </div>
    </a>
  )
}

export default Chatbox;
