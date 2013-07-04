open canopy

open runner

start firefox

let reset _ =
    let seed = new Oak.Controllers.SeedController()
    seed.PurgeDb() |> ignore
    seed.All() |> ignore
    () 

before(reset)

"creating a blog" &&& fun _ ->
    url "http://localhost:3000/"
    "#title" << "A New Blog Post"
    click "create"
    "#blogs" == "A New Blog Post"

run()

quit()