export default function Header() {
    return (
        <header className="bg-[#161616] col-span-2 row-start-1 flex justify-between items-center p-3 text-white">
            <h1>My App</h1>
            <figure className="flex items-center cursor-pointer hover:bg-amber-500">
                <figcaption>User Name</figcaption>
                <img src="https://placehold.co/50x50" alt="Profile Image" className="w-12.5 h-12.5 rounded-full ml-3" />
            </figure>
        </header>
    )
}