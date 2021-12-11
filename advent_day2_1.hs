-- John Hurst
-- 2021-12-12

import Data.Char
import qualified Data.Text as Text
import qualified Data.Text.IO as Text
import System.Environment

data Command = Forward Int | Up Int | Down Int deriving (Show)
data Position = Position Int Int deriving (Show)

update :: Position -> Command -> Position
update (Position h d) (Forward n) = Position (h+n) d
update (Position h d) (Up n) = Position h (d-n)
update (Position h d) (Down n) = Position h (d+n)

result :: Position -> Int
result (Position h d) = h * d

toInt :: Text.Text -> Int 
toInt s = read $ Text.unpack s

parse :: Text.Text -> Command
parse s
    | c == Text.pack "forward" = Forward n 
    | c == Text.pack "up" = Up n
    | otherwise = Down n
    where sl = Text.split isSpace s
          c = head sl
          n = toInt $ head $ tail sl

main = do
    fileName <- fmap head getArgs
    lines <- fmap Text.lines (Text.readFile fileName)
    let commands = fmap parse lines
        position = foldl update (Position 0 0) commands 
    print $ result position
