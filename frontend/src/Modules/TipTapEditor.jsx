import { useEditor, EditorContent, useEditorState, EditorContext } from '@tiptap/react'
import Document from '@tiptap/extension-document'
import Heading from '@tiptap/extension-heading'
import Paragraph from '@tiptap/extension-paragraph'
import Text from '@tiptap/extension-text'
import Bold from '@tiptap/extension-bold'
import Italic from '@tiptap/extension-italic'
import { TableKit } from '@tiptap/extension-table'
import { ListKit } from '@tiptap/extension-list'
import { useState } from 'react'
import { Gapcursor } from '@tiptap/extensions'

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { library } from '@fortawesome/fontawesome-svg-core'

/* import all the icons in Free Solid, Free Regular, and Brands styles */
import { fas } from '@fortawesome/free-solid-svg-icons'
import { far } from '@fortawesome/free-regular-svg-icons'
import { fab } from '@fortawesome/free-brands-svg-icons'

import { useEffect } from 'react'

library.add(fas, far, fab)

function TipTap({ contentJSON, setEditorInstance }) {
    const [dropDownOpen, setDropDownOpen] = useState(false)





    // The extensions are what functionalities the editor has
    // The content is the initial content of the editor
    const editor = useEditor({
        extensions: [
            Document,
            Heading.configure({
                levels: [1, 2]
            }),
            Paragraph,
            Text,
            Bold,
            Italic,
            TableKit.configure({
                table: {
                    resizable: true,
                }
            }),
            ListKit.configure({
                taskItem: {
                    nested: true,
                }
            }),
            Gapcursor
        ],
        content: '',
    })

    if (!editor) return null;

    useEffect(() => {
        if (editor) setEditorInstance(editor)
    }, [editor]) // Run this when the editor gets initialized. If this is not set as a dependency, then the editor will not be set, since editor is a falsy value.



    useEffect(() => {
        if (editor) {
            const currentContent = editor.getJSON();
            const newContent = contentJSON ? JSON.parse(contentJSON) : contentJSON;

            if (JSON.stringify(currentContent) !== JSON.stringify(newContent))
                editor.commands.setContent(newContent);
        }
    }, [editor, contentJSON]);

    return (
        <div className="flex-1 flex flex-col border-2 rounded-md">
            <div className='button-group flex gap-5 bg-[#161616] rounded-t-sm p-2'>
                <button className='cursor-pointer' onClick={() => editor.chain().focus().toggleBold().run()}><FontAwesomeIcon icon="fa-solid fa-bold" /></button>
                <button className='cursor-pointer' onClick={() => editor.chain().focus().toggleItalic().run()}><FontAwesomeIcon icon="fa-solid fa-italic" /></button>
                <button className='cursor-pointer' onClick={() => editor.chain().focus().setHeading({ level: 1 }).run()}>H1</button>
                <button className='cursor-pointer' onClick={() => editor.chain().focus().setHeading({ level: 2 }).run()}>H2</button>
                <button className='cursor-pointer' onClick={() => editor.chain().focus().setParagraph().run()}>p</button>

                {/* table button dropdown */}
                <div className="relative">
                    <button className='cursor-pointer' onClick={() => { dropDownOpen ? setDropDownOpen(false) : setDropDownOpen(true) }}><FontAwesomeIcon icon="fa-solid fa-table" /></button>

                    {/* The dropdown */}
                    <div className={`${dropDownOpen ? 'block' : 'hidden'} absolute bg-red-500 list-none whitespace-nowrap z-50`}>
                        <li className='cursor-pointer hover:bg-red-600'><button className='px-1 py-2 cursor-pointer' onClick={() => editor.chain().focus().insertTable({ rows: 3, cols: 3, withHeaderRow: true }).run()}>Insert Table</button></li>
                        <li className='cursor-pointer hover:bg-red-600'><button className='px-1 py-2 cursor-pointer' onClick={() => editor.chain().focus().addColumnAfter().run()}>Insert Column</button></li>
                        <li className='cursor-pointer hover:bg-red-600'><button className='px-1 py-2 cursor-pointer' onClick={() => editor.chain().focus().addRowAfter().run()}>Insert Row</button></li>
                        <li className='cursor-pointer hover:bg-red-600'><button className='px-1 py-2 cursor-pointer' onClick={() => editor.chain().focus().deleteColumn().run()}>Delete Column</button></li>
                        <li className='cursor-pointer hover:bg-red-600'><button className='px-1 py-2 cursor-pointer' onClick={() => editor.chain().focus().deleteRow().run()}>Delete Row</button></li>
                        <li className='cursor-pointer hover:bg-red-600'><button className='px-1 py-2 cursor-pointer' onClick={() => editor.chain().focus().deleteTable().run()}>Delete Table</button></li>
                    </div>
                </div>

                <button className='cursor-pointer' onClick={() => editor.chain().focus().toggleBulletList().run()}><FontAwesomeIcon icon="fa-solid fa-list-ul" /></button>
                <button className='cursor-pointer' onClick={() => editor.chain().focus().toggleTaskList().run()}><FontAwesomeIcon icon="fa-solid fa-list-check" /></button>

            </div>
            <EditorContent className='flex-1 p-2' editor={editor} />

        </div>
    )
}

export default TipTap
